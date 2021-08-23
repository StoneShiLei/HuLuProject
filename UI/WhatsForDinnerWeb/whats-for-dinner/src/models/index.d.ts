
interface TypeModel {
    id?: string,
    userId?: string,
    typeName: string,
    menuCount?: string,
    menus?: MenuModel[]
}

interface FoodModel {
    id?: string,
    userId?: string,
    foodName: string
}

interface MenuModel {
    id?: string,
    userId?: string,
    menuName: string,
    typeName?: string,
    typeId: string,
    foodNames?: string,
    foodIds: string[],
    isEnabled?: boolean
}