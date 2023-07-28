import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RedeSocial } from '@app/models/RedeSocial';
import { environment } from '@environments/environment';
import { Observable, take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RedeSocialService {

  baseURL = environment.apiURL + 'api/redesSociais';

  constructor(private http: HttpClient) { }

  /**
   *
   * @param origem Precisa passar a palavra 'palestrante' ou a palavra 'evento' - Escrito em minúsculo.
   * @param id Precisa passar o PalestranteId ou o EventoId, dependendo da sua origem.
   * @returns Observable<RedeSocial[]>
   */
  public getRedesSociais(origem: string, id: number): Observable<RedeSocial[]>{
    let URL =
      id === 0
        ? `${this.baseURL}/${origem}`
        : `${this.baseURL}/${origem}/${id}`

        return this.http.get<RedeSocial[]>(URL).pipe(take(1));
  }

    /**
   *
   * @param origem Precisa passar a palavra 'palestrante' ou a palavra 'evento' - Escrito em minúsculo.
   * @param id Precisa passar o PalestranteId ou o EventoId, dependendo da sua origem.
   * @param redesSociais Precisa adicionar Redes Sociais organizadas em RedeSocial[].
   * @returns Observable<RedeSocial[]>
   */
    public saveRedesSociais(origem: string, id: number, redesSociais: RedeSocial[]): Observable<RedeSocial[]>{
      let URL =
        id === 0
          ? `${this.baseURL}/${origem}`
          : `${this.baseURL}/${origem}/${id}`

          return this.http.put<RedeSocial[]>(URL, redesSociais).pipe(take(1));
    }

    /**
   *
   * @param origem Precisa passar a palavra 'palestrante' ou a palavra 'evento' - Escrito em minúsculo.
   * @param id Precisa passar o PalestranteId ou o EventoId, dependendo da sua origem.
   * @param redesSocialId Precisa usar o id da Rede Social.
   * @returns Observable<any> - pois é o retorno da rota.
   */
    public deleteRedeSocial(origem: string, id: number, redesocialId: number): Observable<any>{
      let URL =
        id === 0
          ? `${this.baseURL}/${origem}/${redesocialId}`
          : `${this.baseURL}/${origem}/${id}/${redesocialId}`

          return this.http.delete(URL).pipe(take(1));
    }



}
