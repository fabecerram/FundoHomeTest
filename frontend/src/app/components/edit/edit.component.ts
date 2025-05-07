import { Component, OnInit } from '@angular/core';
import { LoanService } from '../../services/loan.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Loan } from '../../interfaces/loan';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrl: './edit.component.css',
  standalone: false
})
export class EditComponent implements OnInit{
  
  loan: Loan = {
    applicantName: '',
    amount: 0,
    currentBalance: 0,
    status: 'Active',
    id: 0
  };

  options = [{ value: 'Active', label: 'Active' }, { value: 'Paid', label: 'Paid' }];

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router:Router,
    private readonly service: LoanService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(
      param => {
        this.getById(param['id']);
    });
  }

  getById(id: string){
    this.service.getLoan(id).subscribe(
      (data: any) => {
        this.loan = data;
      }
    );

  }

  update() {
    this.service.updateLoan(this.loan).subscribe(
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
