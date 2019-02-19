import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  public products: Product[];

  constructor() { }

  ngOnInit() {
    //get from backend after implementing security
    this.products = [
      { description: 'Bottle of water', quantity: 10, price: 1.5 },
      { description: 'French fries', quantity: 15, price: 2.5 },
      { description: 'Snack', quantity: 7, price: 5 },
    ]
  }
}

interface Product {
  description: string;
  quantity: number;
  price: number;
}
