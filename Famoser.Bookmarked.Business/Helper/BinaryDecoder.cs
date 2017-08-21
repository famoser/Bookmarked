using System;
using System.Text;
using Famoser.FrameworkEssentials.Logging;

namespace Famoser.Bookmarked.Business.Helper
{
    public class BinaryDecoder
    {
        private readonly byte[] _payload;
        private string _result;
        private int _faillureCode = 0;
        public BinaryDecoder(byte[] payload)
        {
            _payload = payload;
        }

        public bool Decode()
        {
            try
            {
                if (_payload == null)
                {
                    _faillureCode = 1;
                    return false;
                }

                if (_payload.Length < 1)
                {
                    _faillureCode = 2;
                    return false;
                }

                //first bit marks file info
                if (_payload[0] == 0)
                {
                    _result = null;
                    return true;
                }
                if (_payload[0] == 1)
                {
                    return DecodeYEnc();
                }
                _faillureCode = 3;
                return false;
            }
            catch (Exception e)
            {
                _faillureCode = 99;
                LogHelper.Instance.LogException(e);
            }
            return false;
        }

        private bool DecodeYEnc()
        {
            _result = "";
            var bytes = new byte[_payload.Length];
            var byteIndex = 0;

            for (int i = 1; i < _payload.Length; i++)
            {
                int current = _payload[i];
                if (current == 61)
                {
                    //escape caracter, so we need next
                    if (++i == _payload.Length)
                    {
                        //stream finished with escape caracter, fail!
                        _faillureCode = 101;
                        return false;
                    }
                    //inverse + 64 mod 256
                    current = _payload[i] - 64;
                    if (current < 0)
                        current += 256;
                }
                //inverse + 42 mod 256
                current -= 42;
                if (current < 0)
                    current += 256;
                bytes[byteIndex++] = (byte) current;
            }

            if (byteIndex > 0)
                _result += Encoding.GetEncoding("ASCII").GetString(bytes, 0, byteIndex);
            return true;
        }

        public string GetFaillure()
        {
            switch (_faillureCode)
            {
                case 0:
                    return "no faillure";
                case 1:
                    return "payload null";
                case 2:
                    return "payload too short";
                case 3:
                    return "no encoding found";
                case 4:
                    return "no faillure";
                case 99:
                    return "exception occurred";
                case 101:
                    return "YEnc: Escape caracter at the end";
                default:
                    return "unknown faillure";
            }
        }

        public string GetResult()
        {
            return _result;
        }
    }
}
