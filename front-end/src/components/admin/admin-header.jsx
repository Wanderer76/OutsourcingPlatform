import headerLogo from "../../images/header-logo.svg"
import React, {useState} from "react";
import LogoBlack from "../../images/logo-black.svg";

const AdminHeader = () => {
    const [popup, setPopup] = useState(<></>);
    function viewPopup() {
        setPopup(<div className="popup-wrapper">
            <div className="popup leave-account-popup">
                <img className="finish-project-popup-logo" src={LogoBlack}/>
                <p className="leave-text">Желаете выйти из аккаунта администратора?</p>
                <div className="popup-buttons buttons">
                    <a className="project-card-btn" href="/" onClick={() => window.sessionStorage.clear()}>Выйти</a>
                    <a className="project-card-btn" onClick={hidePopup}>Отмена</a>
                </div>
            </div>
        </div>)
    }
    function hidePopup() {
        setPopup(<></>)
    }
    return(
        <header className="main-header">
            {popup}
            <div className="logo">
                <a href="#" className="logo-link header-logo">
                    <img className="header-logo" src={headerLogo} alt="Логотип платформы Первый Старт"/>
                </a>

                <div className="dropdown">
                    <a className="auth-link dropbtn" href="#">
                        <div className="nav-username">@ADMIN12</div>
                    </a>
                    <div className="dropdown-content">
                        <a href="/admin">Панель администратора</a>
                        <a href="#" onClick={(e) => {
                            viewPopup();
                            e.preventDefault();
                        }}>Выход</a>
                    </div>
                </div>
            </div>

            <input className="side-menu" type="checkbox" id="side-menu"/>
            <label className="dash" htmlFor="side-menu"><span className="dash-line"></span></label>

            <nav className="nav">
                <ul className="nav-menu">
                    {/*<li className="nav-for-mobile"><a href="#">Личный кабинет</a></li>*/}
                    <li><a href="/admin">Панель администратора</a></li>
                    {/*<li><a href="#">Список пользователей</a></li>*/}
                    <li><a href="/admin/tags-editor">Редактор тегов</a></li>
                    <li className="nav-for-mobile"><a href="#" onClick={(e) => {
                        viewPopup();
                        e.preventDefault();
                    }}>Выход</a></li>
                </ul>
            </nav>
        </header>
    )
}

export default AdminHeader;