import logo from "./../../images/logo-auth-white.svg";

const Content = () => {
    return(
        <div className="sign-in__content">
            <a href="/" className="logo-link">
                <img className="logo" src={logo} alt="Логотип платформы Первый Старт"/>
            </a>
            <h1>Первый старт —<br/>— в вашу карьеру</h1>
            <p>Помогаем талантливым начинающим исполнителям и потенциальным заказчикам найти друг друга</p>
        </div>
    );
}

export default Content;