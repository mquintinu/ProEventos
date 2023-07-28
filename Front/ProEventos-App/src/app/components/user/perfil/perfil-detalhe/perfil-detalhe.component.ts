import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil-detalhe',
  templateUrl: './perfil-detalhe.component.html',
  styleUrls: ['./perfil-detalhe.component.scss']
})
export class PerfilDetalheComponent implements OnInit {
  @Output() changeFormValue = new EventEmitter();

  form: FormGroup;
  userUpdate = {} as UserUpdate;

  constructor(private fb: FormBuilder,
              public accountService: AccountService,
              public palestranteService: PalestranteService,
              private router: Router,
              private toaster: ToastrService,
              private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.validation();
    this.carregarUsuario();
    this.verificaForm();
  }

  get f(): any{
    return this.form.controls;
  }

  public validation(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmarPassword')
    };

    this.form = this.fb.group({
      userName: [''],
      imagemURL: [''],
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

  private carregarUsuario(): void{
    this.spinner.show();
    this.accountService.getUser().subscribe(
      (userRetorno: UserUpdate) => {
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

  private verificaForm(): void {
    this.form.valueChanges
      .subscribe(() => this.changeFormValue.emit({ ... this.form.value}))
  }

  public resetForm(event: any): void{
    event.preventDefault();
    this.form.reset();
  }

  onSubmit(): void{
    this.atualizarUsuario();
  }

  public atualizarUsuario(): void {
    this.userUpdate = { ... this.form.value }
    this.spinner.show();

    if (this.f.funcao.value = 'Palestrante'){
      this.palestranteService.post().subscribe(
        () => this.toaster.success('Função ativada!', 'Sucesso'),
        (error) => {
          this.toaster.error('A função palestrante não pôde ser ativada.', 'Error');
          console.error(error);
        }
      )
    }


    this.accountService.updateUser(this.userUpdate).subscribe(
      () => { this.toaster.success('Usuário atualizado!', 'Sucesso') },
      (error) => {
        this.toaster.error(error.error)
        console.error(error)
      },
      )
      .add(() => this.spinner.hide())
  }

}
