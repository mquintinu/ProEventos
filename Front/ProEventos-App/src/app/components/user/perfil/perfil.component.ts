import { Component, OnInit } from '@angular/core';

import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { environment } from '@environments/environment';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  public usuario = {} as UserUpdate;
  public imagemURL = '';
  public file: File;

  constructor(private spinner: NgxSpinnerService,
              private toastr: ToastrService,
              private accountService: AccountService) {}

  ngOnInit(): void {
    this.usuario.funcao = '';
  }

  public setFormValue(usuario: UserUpdate): void {
    this.usuario = usuario;
    if (this.usuario.imagemURL)
      this.imagemURL = environment.apiURL + `resources/perfil/${this.usuario.imagemURL}`;
    else
      this.imagemURL = './assets/img/perfil.png';
  }

  public get isPalestrante(): boolean {
    return this.usuario.funcao.toLocaleUpperCase() === 'PALESTRANTE';
  }

  onFileChange(ev: any): void{
    const reader = new FileReader();

    reader.onload = (event: any) => this.imagemURL = event.target.result;

    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0]);

    this.uploadImagem();
  }

  private uploadImagem(): void {
    this.spinner.show();
    this.accountService
      .postUpload(this.file)
      .subscribe(
        () => this.toastr.success('Imagem atualizada com sucesso!'),
        (error: any) => {
          this.toastr.error('Erro ao tentar realizar upload de imagem.', 'Erro');
          console.log(error);
        }
    ).add(() => this.spinner.hide())

    // this.accountService
    //   .postUpload(this.file)
    //   .this.service.function
    //     .subscribe(arg => this.property = arg);

  }
}
