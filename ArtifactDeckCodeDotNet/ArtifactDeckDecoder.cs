using System;
using System.Text;

namespace ArtifactDeckCodeDotNet
{
    using static Utils;

    public static class ArtifactDeckDecoder
    {
        /// <summary>
        /// Decodes the a string code into a <see cref="Deck"/> object.
        /// </summary>
        public static Deck DecodeDeck(string deckCode)
        {
            if (deckCode == null)
                throw new ArgumentNullException(nameof(deckCode));

            byte[] bytes = DecodeDeckStringToBytes(deckCode);
            var byteDecoder = new Decoder();

            return byteDecoder.DecodeFromBytes(bytes);
        }

        private static byte[] DecodeDeckStringToBytes(string deckString)
        {
            if (!deckString.StartsWith(Constants.EncodedPrefix))
                throw new ArgumentException($"The {nameof(deckString)} is missing an Artifact Deck Code prefix.");

            string base64Encoding = deckString.Substring(Constants.EncodedPrefix.Length)
                                              .Replace('-', '/')
                                              .Replace('_', '=');

            return Convert.FromBase64String(base64Encoding);
        }

        private class Decoder
        {
            private byte[] _bytes;
            private Deck _deck;
            private int _currentByteIndex;

            private byte _versionAndHeroes;
            private int _version;
            private byte _checksum;
            private int _nameLength;
            private int _cardDataLength;
            private uint _heroCount;

            public Deck DecodeFromBytes(byte[] bytes)
            {
                if (bytes == null)
                    throw new ArgumentNullException(nameof(_bytes));

                InitDecoderState(bytes);

                ReadVersionAndPartialHeroCount();
                ReadChecksum();
                ReadNameLength();
                VerifyChecksum();
                ReadHeroCount();
                ReadHeroCards();
                ReadCards();
                ReadName();

                return _deck;
            }

            private void InitDecoderState(byte[] bytes)
            {
                _bytes = bytes;
                _deck = new Deck();
                _currentByteIndex = 0;
            }

            private void ReadVersionAndPartialHeroCount()
            {
                _versionAndHeroes = _bytes[_currentByteIndex++];
                _version = _versionAndHeroes >> 4;

                if (Constants.CurrentVersion != _version && _version != 1)
                    throw new ArtifactDeckDecoderException("Invalid code version.");
            }

            private void ReadChecksum()
            {
                _checksum = _bytes[_currentByteIndex++];
            }

            private void ReadNameLength()
            {
                _nameLength = 0;
                if (_version > 1)
                    _nameLength = _bytes[_currentByteIndex++];

                _cardDataLength = _bytes.Length - _nameLength;
            }

            private void VerifyChecksum()
            {
                int dataLength = _cardDataLength - _currentByteIndex;
                byte checksum = ShortChecksum(_bytes, Constants.HeaderSize, dataLength);
                if (checksum != _checksum)
                    throw new ArtifactDeckDecoderException("Checksum mismatch.");
            }

            private void ReadHeroCount()
            {
                // Read in the hero count (part of the bits are in the version, but we can overflow bits here)
                if (!TryReadVarEncodedUint32(_versionAndHeroes, 3, _cardDataLength, out _heroCount))
                    throw new ArtifactDeckDecoderException("Missing hero count");
            }

            private void ReadHeroCards()
            {
                uint prevCardBase = 0;
                for (int currHero = 0; currHero < _heroCount; currHero++)
                {
                    if (!TryReadSerializedCard(_cardDataLength, ref prevCardBase, out uint turn, out uint cardId))
                        throw new ArtifactDeckDecoderException("Missing hero data.");

                    _deck.Heroes.Add(new HeroRef(cardId, turn));
                }
            }

            private void ReadCards()
            {
                uint prevCardBase = 0;
                while (_currentByteIndex < _cardDataLength)
                {
                    if (!TryReadSerializedCard(_bytes.Length, ref prevCardBase, out uint cardCount, out uint cardId))
                        throw new ArtifactDeckDecoderException("Missing card data.");

                    _deck.Cards.Add(new CardRef(cardId, cardCount));
                }
            }

            private void ReadName()
            {
                int bytesLeft = _bytes.Length - _currentByteIndex;
                if (_nameLength > 0 && bytesLeft == 0)
                    throw new ArtifactDeckDecoderException("Missing name data.");

                if (_currentByteIndex < _bytes.Length)
                {
                    int nameOffset = _bytes.Length - _nameLength;
                    _deck.Name = Encoding.UTF8.GetString(_bytes, nameOffset, _nameLength);
                }
            }

            private bool TryReadVarEncodedUint32(uint baseValue, int baseBits, int indexEnd, out uint outValue)
            {
                outValue = 0;

                int deltaShift = 0;
                if ((baseBits == 0) || ReadBitsChunk(baseValue, baseBits, deltaShift, ref outValue))
                {
                    deltaShift += baseBits;

                    while (true)
                    {
                        // Check for end of the memory block
                        if (_currentByteIndex > indexEnd)
                            return false;

                        // Read the bits from this next byte and see if we are done
                        byte nextByte = _bytes[_currentByteIndex++];
                        if (!ReadBitsChunk(nextByte, 7, deltaShift, ref outValue))
                            break;

                        deltaShift += 7;
                    }
                }

                return true;
            }

            /// <summary>
            /// Handles decoding a card that was serialized.
            /// </summary>
            private bool TryReadSerializedCard(int indexEnd, ref uint prevCardBase, out uint outCount, out uint outCardId)
            {
                // Initialize out values
                outCount = 0;
                outCardId = 0;

                // Check for end of the memory block
                if (_currentByteIndex > indexEnd)
                    return false;

                // Header contains the count (2 bits), a continue flag, and 5 bits of offset data.
                // If we have 11 for the count bits we have the count encoded after the offset.
                byte header = _bytes[_currentByteIndex++];
                bool hasExtendedCount = ((header >> 6) == 0x03);

                // Read in the delta, which has 5 bits in the header, then additional bytes while the value is set
                uint cardDelta = 0;
                if (!TryReadVarEncodedUint32(header, 5, indexEnd, out cardDelta))
                    return false;

                outCardId = prevCardBase + cardDelta;

                // Now parse the count if we have an extended count
                if (hasExtendedCount)
                {
                    if (!TryReadVarEncodedUint32(0, 0, indexEnd, out outCount))
                        return false;
                }
                else
                {
                    // The count is just the upper two bits + 1 (since we don't encode zero)
                    outCount = (uint)(header >> 6) + 1;
                }

                // Update our previous card before we do the remap, since it was encoded without the remap
                prevCardBase = outCardId;

                return true;
            }

            /// <summary>
            /// Reads out a var-int encoded block of bits; returns true if another chunk should follow;
            /// </summary>
            private static bool ReadBitsChunk(uint chunk, int numBits, int currShift, ref uint outBits)
            {
                uint continueBit = (1u << numBits);
                uint newBits = chunk & (continueBit - 1);
                outBits |= (newBits << currShift);

                return (chunk & continueBit) != 0;
            }
        }
    }
}
