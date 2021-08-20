
interface TypeModel {
    id: string,
    userId: string,
    typeName: string
}

interface FoodModel {
    id: string,
    userId: string,
    foodName: string
}

interface MenuModel {
    id: string,
    userId: string,
    menuName: string,
    typeName: string,
    foodNames: string
}