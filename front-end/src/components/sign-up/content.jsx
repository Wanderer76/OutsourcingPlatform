import logo from "./../../images/logo-auth-white.svg";
import { constructorRole, customerRole } from "./constants";


const SingUpContent = (props) => {
    return (
        <div className="sign-up__content">
            <a href="#" className="logo-link">
                <img className="logo" src={logo} alt="Логотип платформы Первый Старт"/>
            </a>

            <div className="sign-up__content content-text">
                <h1>Исполнитель и заказчик — в чем разница?</h1>
                <p>Помогаем талантливым начинающим исполнителям и потенциальным заказчикам найти друг друга</p>
                <p>Заказчик — предприниматель или компания, которые нуждаются в выполнении каких-то задач с помощью исполнителей.</p>
            </div>

            <div className="roles">
                <p>Выберите роль на платформе:</p>
                <div className="roles-radio">
                    <input className="visually-hidden" type="radio" id="contractor" name="role" value="contractor" onChange={props.onChangeFunc}/>
                    {props.role === constructorRole ?
                        <label className="role-btn checked" htmlFor="contractor">Исполнитель</label> :
                        <label className="role-btn" htmlFor="contractor">Исполнитель</label> 
                    }
                    <input className="visually-hidden" type="radio" id="customer" name="role" value="customer" onChange={props.onChangeFunc}/>
                    {props.role === customerRole ?
                        <label className="role-btn checked" htmlFor="customer">Заказчик</label> :
                        <label className="role-btn" htmlFor="customer">Заказчик</label> }
                </div>
            </div>
        </div> 
    );
    
}



export default SingUpContent;