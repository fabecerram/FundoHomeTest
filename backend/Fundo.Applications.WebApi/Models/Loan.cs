using System;
using System.Collections.Generic;

namespace Fundo.Applications.WebApi.Models;

public partial class Loan
{
    public int Id { get; set; }

    public string ApplicantName { get; set; }

    public decimal? Amount { get; set; }

    public decimal? CurrentBalance { get; set; }

    public string Status { get; set; }
}