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