using System;
using System.Text;
using Famoser.FrameworkEssentials.Logging;

namespace Famoser.Bookmarked.Business.Helper
{
    public class StringDecoder
    {
        private readonly string _payload;
        private byte[] _result;
        private int _faillureCode = 0;
        public StringDecoder(string payload)
        {
            _payload = payload;
        }

        public bool Encode()
        {
            try
            {
                if (_payload == null)
                {
                    _result = new byte[] { 0 };
                    return true;
                }

                return EncodeYEnc();
            }
            catch (Exception e)
            {
                _faillureCode = 99;
                LogHelper.Instance.LogException(e);
            }
            return false;
        }

        private bool EncodeYEnc()
        {
            var content = Encoding.GetEncoding("ASCII").GetBytes(_payload);
            _result = new byte[_payload.Length * 2];
            var byteIndex = 0;

            for (int i = 0; i < content.Length; i++)
            {
                int current = content[i];
                //check for escapes
                if (true)//current == 0 || current == 10 || current == )
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
                _result[byteIndex++] = (byte)current;
            }

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

        public byte[] GetResult()
        {
            return _result;
        }
    }
}
