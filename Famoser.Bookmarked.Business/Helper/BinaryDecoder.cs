using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Famoser.FrameworkEssentials.Logging;

namespace Famoser.Bookmarked.Business.Helper
{
    public class BinaryDecoder
    {
        private byte[] _payload;
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

                if (_payload[0] == 0)
                {
                    return EncodeInYEnc();
                }
                _faillureCode = 3;
                return false;
            }
            catch (Exception e)
            {
                _faillureCode = 99;
                LogHelper.Instance.LogException(e);
            }
        }

        private bool EncodeInYEnc()
        {
            _result = "";
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
                    }
                    current = _payload[i] - 64;
                    if (current < 0)
                        current += 256;
                }
            }
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
