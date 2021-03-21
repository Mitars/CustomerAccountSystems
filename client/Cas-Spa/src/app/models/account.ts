import { Transaction } from './transaction';

export interface Account {
  id: number;
  name: string;
  created: Date;
  transactions: Transaction[];
}
