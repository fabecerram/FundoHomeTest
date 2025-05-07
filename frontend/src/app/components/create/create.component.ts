import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LoanService } from '../../services/loan.service';
import { Loan } from '../../interfaces/loan';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrl: './create.component.css',
  standalone: false
})
export class CreateComponent {

  newLoan: Loan = {
    applicantName: '',
    amount: 0,
    currentBalance: 0,
    status: 'Active',
    id: 0
  };

  constructor(private readonly service: LoanService, private readonly router: Router) { }

  create() {
    this.newLoan.currentBalance = this.newLoan.amount;

    this.service.addLoan(this.newLoan).subscribe(
      () => {
        this.router.navigate(['/']);
        //this.router.navigate(["home"])
      }
    );
  }

  cancel(){
    this.router.navigate(['/']);
    //this.router.navigate(["home"])
  }
}
