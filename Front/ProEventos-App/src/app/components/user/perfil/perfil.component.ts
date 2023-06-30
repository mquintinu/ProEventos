import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorField } from '@app/helpers/ValidatorField';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.css']
})
export class PerfilComponent implements OnInit {

  form: FormGroup;

  constructor(private fb: FormBuilder) {}

  get f(): any{
    return this.form.controls;
  }

  ngOnInit(): void {
    this.validation();
  }

  public validation(): void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('senha', 'confirmarSenha')
    };

    this.form = this.fb.group({
      titulo: ['', [Validators.required]],
      primeiroNome: ['', [Validators.required]],
      ultimoNome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      telefone: ['', Validators.required],
      funcao: ['', Validators.required],
      senha: ['', [Validators.required, Validators.minLength(6)]],
      confirmarSenha: ['', [Validators.required, Validators.minLength(6)]]
    },
    formOptions);
  }

  public resetForm(event: any): void{
    event.preventDefault();
    this.form.reset();
  }

}
