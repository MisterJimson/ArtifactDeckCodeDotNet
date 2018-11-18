using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtifactDeckCodeDotNet
{
    using static Utils;

    public static class ArtifactDeckEncoder
    {
        /// <summary>
        /// Encodes a <see cref="Deck"/> object into a string code.
        /// Note that signature cards for heroes SHOULD NOT be included in the deck.
        /// </summary>
        public static string EncodeDeck(Deck deck)
        {
            if (deck == null)
                throw new ArgumentNullException(nameof(deck));

            var byteEncoder = new Encoder();
            byte[] bytes = byteEncoder.EncodeToBytes(deck);

            return EncodeDeckBytesToString(bytes);
        }

        private static string EncodeDeckBytesToString(byte[] bytes)
        {
            string base64Encoding = Convert.ToBase64String(bytes);
            string deckString = Constants.EncodedPrefix + base64Encoding;

            return deckString.Replace('/', '-')
                             .Replace('=', '_');
        }

        private class Encoder
        {
            private byte[] _deckNameUTF8;
            private HeroRef[] _sortedHeroes;
            private CardRef[] _sortedCards;
            private List<byte> _bytes;
            private int _checksumPosn;
            private int _preStringByteCount;

            public byte[] EncodeToBytes(Deck deck)
            {
                if (deck == null)
                    throw new ArgumentNullException(nameof(deck));

                InitEncoderState(deck);

                WriteVersionAndPartialHeroCount();
                WriteChecksumPlaceholder();
                WriteNameLength();
                WriteRestOfHeroCount();
                WriteHeroCards();
                WriteCards();
                WriteName();
                UpdateChecksum();

                return _bytes.ToArray();
            }

            private void InitEncoderState(Deck deck)
            {
                string sanitizedName = Sanitize(deck.Name ?? string.Empty);
                string truncatedName = PrefixWithMaxBytes(sanitizedName, Constants.DeckNameMaxBytes, Encoding.UTF8);
                _deckNameUTF8 = Encoding.UTF8.GetBytes(truncatedName);

                _sortedHeroes = deck.Heroes.OrderBy(x => x.Id).ToArray();
                _sortedCards = deck.Cards.OrderBy(x => x.Id).ToArray();

                _bytes = new List<byte>();
            }

            private void WriteVersionAndPartialHeroCount()
            {
                uint heroCountLowBits = ExtractNBitsWithCarry((uint)_sortedHeroes.Length, 3);
                uint value = Constants.CurrentVersion << 4 | heroCountLowBits;
                AddByte(value);
            }

            private void WriteChecksumPlaceholder()
            {
                _checksumPosn = _bytes.Count;
                AddByte(0);
            }

            private void WriteNameLength()
            {
                AddByte((uint)_deckNameUTF8.Length);
            }

            private void WriteRestOfHeroCount()
            {
                AddRemainingNumberToBuffer((uint)_sortedHeroes.Length, 3);
            }

            private void WriteHeroCards()
            {
                uint prevCardId = 0;
                foreach (HeroRef card in _sortedHeroes)
                {
                    if (card.Turn == 0)
                        throw new ArtifactDeckEncoderException("Unable to encode hero card with a turn of 0.");

                    AddCardToBuffer(card.Turn, card.Id - prevCardId);
                    prevCardId = card.Id;
                }
            }

            private void WriteCards()
            {
                uint prevCardId = 0;
                foreach (CardRef card in _sortedCards)
                {
                    if (card.Id == 0)
                        throw new ArtifactDeckEncoderException("Unable to encode card with an id of 0.");
                    else if (card.Count == 0)
                        throw new ArtifactDeckEncoderException("Unable to encode card with a count of 0.");

                    AddCardToBuffer(card.Count, card.Id - prevCardId);
                    prevCardId = card.Id;
                }
            }

            private void WriteName()
            {
                // need to remember for later checksum calculation
                _preStringByteCount = _bytes.Count;

                foreach (byte nameByte in _deckNameUTF8)
                {
                    AddByte(nameByte);
                }
            }

            private void UpdateChecksum()
            {
                int dataLength = _preStringByteCount - Constants.HeaderSize;
                _bytes[_checksumPosn] = ShortChecksum(_bytes, Constants.HeaderSize, dataLength);
            }

            private void AddByte(uint b)
            {
                _bytes.Add(Convert.ToByte(b));
            }

            /// <summary>
            /// Helper to write the rest of a number into a buffer.
            /// This will first strip the specified N bits off, and then write a series of bytes
            /// in the structure of 1 overflow bit and 7 data bits.
            /// </summary>
            private void AddRemainingNumberToBuffer(uint value, int alreadyWrittenBits)
            {
                value >>= alreadyWrittenBits;
                int numBytes = 0;
                while (value > 0)
                {
                    uint nextByte = ExtractNBitsWithCarry(value, 7);
                    value >>= 7;
                    AddByte(nextByte);

                    numBytes++;
                }
            }

            private void AddCardToBuffer(uint count, uint value)
            {
                if (count == 0)
                    throw new InvalidOperationException($"The {nameof(count)} should never be zero.");

                int countBytesStart = _bytes.Count;

                // Determine our count.
                // We can only store 2 bits, and we know the value is at least one,
                // so we can encode values 1-5. However, we set both bits to indicate
                // an extended count encoding.
                uint firstByteMaxCount = 0x03;
                bool extendedCount = (count - 1) >= firstByteMaxCount;

                // Determine our first byte, which contains our count, a continue flag,
                // and the first few bits of our value.
                uint firstByteCount = extendedCount ? firstByteMaxCount : /*( uint8 )*/(count - 1);
                uint firstByte = (firstByteCount << 6);
                firstByte |= ExtractNBitsWithCarry(value, 5);

                AddByte(firstByte);

                // Now continue writing out the rest of the number with a carry flag.
                AddRemainingNumberToBuffer(value, 5);

                // Now if we overflowed on the count, encode the remaining count.
                if (extendedCount)
                {
                    AddRemainingNumberToBuffer(count, 0);
                }

                // Sanity check
                if (_bytes.Count - countBytesStart > 11)
                    throw new InvalidOperationException("Too many bytes written for card.");
            }

            private static uint ExtractNBitsWithCarry(uint value, int numBits)
            {
                uint limitBit = 1u << numBits;
                uint result = value & (limitBit - 1);
                if (value >= limitBit)
                {
                    result |= limitBit;
                }

                return result;
            }
        }
    }
}
