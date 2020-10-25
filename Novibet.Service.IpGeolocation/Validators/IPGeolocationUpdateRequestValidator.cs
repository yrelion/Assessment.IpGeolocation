using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation;
using Humanizer;
using Novibet.Service.IpGeolocation.Common.Models;

namespace Novibet.Service.IpGeolocation.Validators
{
    public class IPGeolocationUpdateListRequestValidator : AbstractValidator<List<IPGeolocationUpdateRequest>>
    {
        public IPGeolocationUpdateListRequestValidator()
        {
            RuleForEach(list => list).SetValidator(new IPGeolocationUpdateRequestValidator());
        }
    }

    public class IPGeolocationUpdateRequestValidator : AbstractValidator<IPGeolocationUpdateRequest>
    {
        public IPGeolocationUpdateRequestValidator()
        {
            RuleFor(x => x.Ip)
                .Must(x => Regex.IsMatch(x,
                    "^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$")).WithMessage(request => "")
                .WithMessage(x => Resources.ValidationErrors.IP_InvalidFormat.FormatWith(x.Ip));
        }
    }
}
