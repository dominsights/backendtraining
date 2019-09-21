import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  public products: Product[];

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  ngOnInit() {
    this.http.get<Product[]>(this.baseUrl + 'api/Product/Products').subscribe(result => {
      this.products = result;
    }, error => console.error(error));
  }
}

interface Product {
  description: string;
  quantity: number;
  price: number;
}
