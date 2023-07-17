import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.css']
})
export class PerfilComponent implements OnInit {

  form: FormGroup;
  userUpdate = {} as UserUpdate;

  constructor(private fb: FormBuilder,
              public accountService: AccountService,
              private router: Router,
              private toaster: ToastrService,
              private spinner: NgxSpinnerService) {}

  get f(): any{
    return this.form.controls;
  }

  onSubmit(): void{
    this.atualizarUsuario();
  }

  public atualizarUsuario(): void {
    this.userUpdate = { ... this.form.value }
    this.spinner.show();
    this.accountService.updateUser(this.userUpdate).subscribe(
      () => { this.toaster.success('Usuário atualizado!', 'Sucesso') },
      (error) => {
        this.toaster.error(error.error)
        console.error(error)
      },
      )
      .add(() => this.spinner.hide())
  }

  ngOnInit(): void {
    this.validation();
    this.carregarUsuario();
  }

  private carregarUsuario(): void{
    this.spinner.show();
    this.accountService.getUser().subscribe(
      (userRetorno: UserUpdate) => {
        console.log(userRetorno);
        this.userUpdate = userRetorno;
        this.form.patchValue(this.userUpdate);
        this.toaster.success('Usuário carregado com sucesso.', 'Sucesso');
      },
      (error) => {
        console.log(error);
        this.toaster.error('Erro ao carregar usuário', 'Erro');
        this.router.navigate(['/dashboard']);
      }
    )
    .add(() => this.spinner.hide());
  }

  public validation(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmarPassword')
    };

    this.form = this.fb.group({
      userName: [''],
      titulo: ['NaoInformado', [Validators.required]],
      primeiroNome: ['', [Validators.required]],
      ultimoNome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      descricao: ['', Validators.required],
      funcao: ['NaoInformado', Validators.required],
      password: ['', [Validators.minLength(6)]],
      confirmarPassword: ['', [Validators.minLength(6)]]
    },
    formOptions);
  }

  public resetForm(event: any): void{
    event.preventDefault();
    this.form.reset();
  }

}
