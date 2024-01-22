import Logo from "../../images/footer-logo.svg"

const Footer = () => {
    return(
    <footer className="main-footer">
        <div className="footer-nav">
            <a href="/404">О системе</a>
            <a href="/executors/search">Исполнители</a>
            <a href="/tasks/search">Заказы</a>
            <a href="/404">Скоро на платформе</a>
            <a className="disabled" href="#">Конкурсы<div className="soon-tag">soon</div></a>
            <a className="disabled" href="#">Вакансии<div className="soon-tag">soon</div></a>
            <a className="disabled" href="#">Стажировки<div className="soon-tag">soon</div></a>
        </div>
        <div className="footer-logo">
            <a className="footer-logo-link">
                <img src={Logo} alt="Логотип платформы Первый Старт"/>
            </a>
            <a className="copyright" href="/404">Политика конфиденциальности</a>
            <a className="copyright" href="/404">Первый старт© 2024 Все права защищены</a>
        </div>
    </footer>)
}

export default Footer;
