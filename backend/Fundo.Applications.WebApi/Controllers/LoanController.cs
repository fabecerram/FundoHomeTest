using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fundo.Applications.WebApi.Constants;
using Fundo.Applications.WebApi.Models;

namespace Fundo.Applications.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanController : ControllerBase
    {
        private readonly FundoLoanContext _context;
        private readonly ILogger<AuthController> _logger;

        public LoanController(FundoLoanContext context, ILogger<AuthController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Loan>>> GetLoans() {
            try {
                List<Loan> loans = await _context.Loans.ToListAsync();

                return Ok(loans);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update loan. Error : {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Loan>> GetLoanById(int id)
        {
            try
            {
                Loan loan = await _context.Loans.FindAsync(id);

                if (loan == null)
                    return NotFound("Loan does not exist");

                return Ok(loan);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get loan. Error : {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Loan>> AddLoan(Loan loan) {
            try {
                _context.Loans.Add(loan);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created, loan);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to create loan. Error : {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Loan>> UpdateLoan(int id, Loan loan) {

            try {
                var updatedLoan = await _context.Loans.FindAsync(id);

                if (updatedLoan != null)
                {
                    updatedLoan.ApplicantName = loan.ApplicantName;
                    updatedLoan.CurrentBalance = loan.CurrentBalance;
                    updatedLoan.Amount = loan.Amount;
                    updatedLoan.Status = loan.Status;

                    await _context.SaveChangesAsync();
                    
                    return Ok(updatedLoan);
                }
                else
                {
                    _logger.LogError("Error: Loan does not exist - Loan ID:" + id);
                    return NotFound("Loan does not exist");
                }  
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to update loan. Error : {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("/{id}/payment")]
        public async Task<ActionResult<Loan>> RegisterLoanPayment(int id)
        {
            /*I overlooked it before, I had no info about the payments, I don't know if it is fixed or variable rate, 
             * so I will assume that we have a fixed fee of 50USD. The ideal is to add to the model a field where 
             * the fixed fee for each loan is defined.*/
            try
            {
                var fidexRate = 50;
                var updatedLoan = await _context.Loans.FindAsync(id);

                if (updatedLoan == null)
                {
                    _logger.LogError("Error: Loan does not exist - Loan ID:" + id);
                    return NotFound("Loan does not exist");
                }

                /*I verify if the loan balance is greater than the fixed rate, and discount the payment, affecting 
                 * only the balance, when the balance is less, since it is not linked to a bank account, I simply 
                 * assume that the remaining balance of the loan has been paid, and leave the balance at zero, and it is marked as paid. */
                if (updatedLoan.CurrentBalance > fidexRate)
                    updatedLoan.CurrentBalance = updatedLoan.CurrentBalance - fidexRate;
                else {
                    updatedLoan.CurrentBalance = 0;
                    updatedLoan.Status = "Paid";
                }

                await _context.SaveChangesAsync();

                return Ok(updatedLoan);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to register payment. Error : {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult> DeleteUser(int id) {
            try {
                var deletedLoan = await _context.Loans.FindAsync(id);

                if (deletedLoan == null)
                    return NotFound();

                _context.Loans.Remove(deletedLoan);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to delete loan. Error : {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
