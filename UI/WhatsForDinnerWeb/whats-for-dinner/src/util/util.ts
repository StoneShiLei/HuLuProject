import { get } from "../api/http";
import url from "../api/url";

export function checkUserAuth(): boolean {
    get<boolean>(url.UserAuth, {}).then((res) => {
        return res.statusCode == 200;
    }).catch((err) => {
        return false;
    })

    return false;
}