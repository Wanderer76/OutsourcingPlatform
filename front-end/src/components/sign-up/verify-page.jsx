import SingUpContent from "./content";
import {constructorRole, customerRole} from "./constants";
import CustomerForm from "./customer-form";
import ConstructorForm from "./constructor-form";
import logo from "../../images/logo-auth-white.svg";


export function VerifyPage() {
    return (
        <div className="auth-wrapper">
            <div className="sign-up">
                <div className="sign-up__content">
                    <a href="#" className="logo-link">
                        <img className="logo" src={logo} alt="Логотип платформы Первый Старт"/>
                    </a>
                    <div className="sign-up__content content-text">
                        <h1>Вы успешно зарегистированы</h1>
                    </div>
                </div>
                <div>
                    <h2>
                        Вам на почту было отправленно письмо со ссылкой. Перейдите по ней чтобы активировать акаунт
                    </h2>
                </div>

            </div>
        </div>
    )
}