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

        private DateTimeOffset _expireDate;
        public DateTimeOffset ExpireDate
        {
            get { return _expireDate; }
            set { Set(ref _expireDate, value); }
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

        public override void SetDefaultValues()
        {
            ExpireDate = DateTimeOffset.Now;
        }

        public override void SetExampleValues()
        {
            CardholderName = "DENNIS HERMAN";
            ExpireDate = DateTimeOffset.Now;
            IssuingBank = "Alternative";
            IssuingBankNumber = "6123";
            Number = "9123-1231-2314-2412";
            Pin = "1234";
            VerificationNumber = "123";
        }

        public override CsvExportEntry ConvertToCsvExportEntry()
        {
            var entry = base.ConvertToCsvExportEntry();

            entry.CardholderName = CardholderName;
            entry.Number = Number;
            entry.VerificationNumber = VerificationNumber;
            entry.Pin = Pin;
            entry.ExpireDate = ExpireDate.ToString("d");
            entry.IssuingBank = IssuingBank;
            entry.IssuingBankNumber = IssuingBankNumber;

            return entry;
        }
    }
}
