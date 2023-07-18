export class Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPage: number;
}


export class PaginatedResult<T>{
  result: T;
  pagination: Pagination;
}
