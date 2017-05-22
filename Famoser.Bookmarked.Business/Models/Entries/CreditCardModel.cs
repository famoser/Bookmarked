using System;
using Famoser.Bookmarked.Business.Enum;

namespace Famoser.Bookmarked.Business.Models.Entries
{
    public class CreditCardModel : WebpageModel
    {
        private string _cardholderName;
        public string CardholderName
        {
            get { return _cardholderName; }
            set { Set(ref _cardholderName, value); }
        }

        private string _number;
        public string Number
        {
            get { return _number; }
            set { Set(ref _number, value); }
        }

        private string _verificationNumber;
        public string VerificationNumber
        {
            get { return _verificationNumber; }
            set { Set(ref _verificationNumber, value); }
        }

        private string _pin;
        public string Pin
        {
            get { return _pin; }
            set { Set(ref _pin, value); }
        }

        private DateTime _expiryDate;
        public DateTime ExpiryDate
        {
            get { return _expiryDate; }
            set { Set(ref _expiryDate, value); }
        }

        private string _issuingBank;
        public string IssuingBank
        {
            get { return _issuingBank; }
            set { Set(ref _issuingBank, value); }
        }

        private string _issuingBankNumber;
        public string IssuingBankNumber
        {
            get { return _issuingBankNumber; }
            set { Set(ref _issuingBankNumber, value); }
        }

        public override ContentType GetContentType()
        {
            return ContentType.CreditCard;
        }
    }
}
