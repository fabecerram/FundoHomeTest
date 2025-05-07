import { Component } from '@angular/core';
import { LoanService } from '../../services/loan.service';
import { Loan } from '../../interfaces/loan';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  standalone: false
})
export class HomeComponent {
  loanList: Loan[] = [];

  constructor(private readonly service:LoanService){
    this.getLoanList();
  }

  getLoanList(){
    this.service.getLoans().subscribe(
      data => {
        this.loanList = data;
      }
    );
  }

  removeLoan(id:number){
    console.log('Rejected - Only an Administrator can delete loans.');
    //TODO: Implement delete method, but need to have role control, only Admin.
  }
}
