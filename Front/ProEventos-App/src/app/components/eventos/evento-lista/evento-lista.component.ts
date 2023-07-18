import { Component, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { environment } from '@environments/environment';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { Subject, debounceTime } from 'rxjs';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent {

  modalRef?: BsModalRef;
  public eventos: Evento[] = [];
  public larguraImagem = 150;
  public margemImagem = 2;
  public exibirImagem = false;
  public eventoId = 0;
  public pagination = {} as Pagination;

  termoBuscaChanged: Subject<string> = new Subject<string>();

  constructor(private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router) {}

    public ngOnInit(): void {
      this.pagination = { currentPage: 1, itemsPerPage: 3, totalItems: 1 } as Pagination;
      this.carregarEventos();
    }

    public alterarImagem() : void{
      this.exibirImagem = !this.exibirImagem;
    }

    public filtrarEventos(evt: any): void {

      if (this.termoBuscaChanged.observers.length === 0){

        this.termoBuscaChanged.pipe(debounceTime(500)).subscribe(
          filtrarPor => {
            this.spinner.show();
            this.eventoService.getEvento(this.pagination.currentPage,
              this.pagination.itemsPerPage,
              filtrarPor
              ).subscribe(
                (paginatedResult: PaginatedResult<Evento[]>) => {
                  this.eventos = paginatedResult.result;
                  this.pagination = paginatedResult.pagination;
                },
                (error: any) => {
                  this.spinner.hide();
                  this.toastr.error('Erro ao carregar os eventos!', 'Erro!');
                },
                ).add(() => this.spinner.hide());
              }
            )
          }
        this.termoBuscaChanged.next(evt.value);
        }

          public mostraImagem(imagemURL: string): string{
            return (imagemURL !== '')
            ? `${environment.apiURL}resources/images/${imagemURL}`
            : 'assets/img/sem-imagem.png';
          }

          public carregarEventos(): void {
            this.spinner.show();
            this.eventoService.getEvento(this.pagination.currentPage,
              this.pagination.itemsPerPage).subscribe(
                (paginatedResult: PaginatedResult<Evento[]>) => {
                  this.eventos = paginatedResult.result;
                  this.pagination = paginatedResult.pagination;
                },
                (error: any) => {
                  this.spinner.hide();
                  this.toastr.error('Erro ao carregar os eventos!', 'Erro!');
                },
                ).add(() => this.spinner.hide());
              }

              openModal(event: any, template: TemplateRef<any>, eventoId: number): void {
                event.stopPropagation();
                this.eventoId = eventoId;
                this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
              }

              public pageChanged(event): void {
                this.pagination.currentPage = event.page;
                this.carregarEventos();
              }

              confirm(): void {
                this.modalRef?.hide();
                this.spinner.show();

                this.eventoService.deleteEvento(this.eventoId).subscribe(
                  (result: any) => {
                    console.log(result);
                    this.toastr.success(`O Evento (Cód.: ${this.eventoId}) foi deletado com sucesso!`, 'Deletado!');
                    this.carregarEventos();
                  },
                  (error: any) => {
                    console.error(error);
                    this.toastr.error(`Erro ao tentar deletar o evento (Cód.: ${this.eventoId})`);
                  }
                  ).add(() => this.spinner.hide());
                }

                decline(): void {
                  this.modalRef?.hide();
                }

                detalheEvento(id: number): void {
                  this.router.navigate([`eventos/detalhe/${id}`]);
                }
              }
