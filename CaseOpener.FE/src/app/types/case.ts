import { Item } from "./item"

export interface Case{
    id: number,
    name: string,
    imageUrl: string,
    price: number,
    categoryName: string
}

export interface CaseDetails{
    case: Case,
    items: Item[]
}

export interface CaseUser{
    id: number,
    caseId: number,
    userId: string, 
    quantity: number,
    case: Case
}

export interface CaseItem{
    id: number,
    caseId: number; 
    itemId: number; 
    probability: number
}

export interface OpenedCase{
    id: number,
    userId: string,
    case: Case,
    item: Item,
    dateOpened: string
  }