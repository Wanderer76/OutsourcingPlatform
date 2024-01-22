import { showPassword } from "./constructor-validation-funcs";
import { sendData } from "./send-data";
import { initializeMasks, updateInnMask } from "./masks-checker";

const CustomerForm = () => {
  document.onchange = () => {
    initializeMasks();
  }

  document.onreadystatechange = () => {
    initializeMasks();
  }
    return(
        <form className="sign-up__form">
      <h2>Регистрация <strong className="blue">заказчика</strong></h2>
      <div className="form-blocks customer">
        <div className="form-block form-block-2">
          <div className="auth__field">
            <p>Фамилия</p>
            <input className="auth_input" type="text" placeholder="Фамилия" name="surname" id="surname" />
            <label className="sign-in__label visually-hidden" htmlFor="surname">Фамилия</label>
            <p className="input-error__text visually-hidden" id="surname-error">Заполните поле</p>
          </div>

          <div className="auth__field">
            <p>Имя</p>
            <input className="auth_input" type="text" placeholder="Имя" name="name" id="name" />
            <label className="sign-in__label visually-hidden" htmlFor="name">Имя</label>
            <p className="input-error__text visually-hidden" id="name-error">Заполните поле</p>
          </div>

          <div className="auth__field">
            <p>Отчество</p>
            <input className="auth_input" type="text" placeholder="Отчество" name="second-name" id="second-name" />
            <label className="sign-in__label visually-hidden" htmlFor="second-name">Отчество</label>
            <p className="input-error__text visually-hidden" id="second-name-error">Заполните поле</p>
          </div>

          <div className="auth__field field-check-box">
            <input className="auth__field sign-in__check-box" type="checkbox" name="no-second-name" id="no-second-name" />
            <label className="sign-in__label" htmlFor="no-second-name">Нет отчества</label>
          </div>

          <div className="auth__field">
            <p>Название организации</p>
            <input className="auth_input" type="text" placeholder="Название организации" name="company-name" id="company-name" />
            <label className="sign-in__label visually-hidden" htmlFor="company-name">Название организации</label>
            <p className="input-error__text visually-hidden" id="company-name-error">Заполните поле</p>
          </div>

          <div className="auth__field">
            <p>Адрес</p>
            <input className="auth_input" type="text" placeholder="Адрес" name="address" id="address" />
            <label className="sign-in__label visually-hidden" htmlFor="address">Адрес</label>
            <p className="input-error__text visually-hidden" id="address-error">Заполните поле</p>
          </div>

          <div className="auth__field">
            <p>ИНН</p>
            <input className="auth_input" type="text" placeholder="XXX-XXX-XXXX" name="inn" id="inn" />

            <label className="sign-in__label visually-hidden" htmlFor="inn">Индивидуальный налоговый номер</label>
            <p className="input-error__text visually-hidden" id="inn-error">Error text (Текст ошибки)!</p>

            {/*<input className="auth__field sign-in__check-box" type="checkbox" name="is-individual" id="is-individual" onChange={updateInnMask}/>*/}
            {/*<label className="sign-in__label" htmlFor="is-individual">Физ. лицо или ИП</label>*/}

            {/*<div className="auth__field field-check-box">*/}
            {/*  <input className="auth__field sign-in__check-box" type="checkbox" name="is-individual" id="is-individual" onChange={updateInnMask}/>*/}
            {/*  <label className="sign-in__label" htmlFor="is-individual">Физ. лицо или ИП</label>*/}
            {/*</div>*/}
          </div>
        </div>


        <div className="form-block form-block-2">
          <div className="auth__field">
            <p>Username</p>
            <input className="auth_input" type="text" placeholder="@username" name="username" id="username" />
            <label className="sign-in__label visually-hidden" htmlFor="username">Username</label>
            <p className="input-error__text visually-hidden" id="username-error">Error text (Текст ошибки)!</p>
          </div>

          <div className="auth__field">
            <p>Телефон</p>
            <input className="auth_input" type="tel" placeholder="+7-XXX-XXX-XX-XX" name="phone" id="phone" />
            <label className="sign-in__label visually-hidden" htmlFor="phone">Телефон</label>
            <p className="input-error__text visually-hidden" id="phone-error">Error text (Текст ошибки)!</p>
          </div>

          <div className="auth__field">
            <p>Email</p>
            <input className="auth_input" type="email" placeholder="your@email.com" name="email" id="email"/>
            <label className="sign-in__label visually-hidden" htmlFor="email">Email</label>
            <p className="input-error__text visually-hidden" id="email-error">Error text (Текст ошибки)!</p>
          </div>

          <div className="password-block">
            <div className="auth__field password">
              <p className="error">Пароль</p>
              <div className="password-with-control">
                <input className="auth_input error" type="password" placeholder="password" name="user-password"
                       id="user-password"/>
                  <a href="#" className="password-control"
                     onClick={() =>
                     {
                       let inp = document.getElementById('user-password');
                       inp.type = inp.type === "text" ? "password" : "text"
                     }}></a>
              </div>
              <label className="sign-in__label visually-hidden" htmlFor="user-password">Пароль</label>
              <p className="input-error__text visually-hidden" id="password-error">Длина пароля должна быть от 6 символов</p>
            </div>


            <div className="auth__field password-repeat">
              <p className="error">Повторите пароль</p>
              <div className="password-with-control">
                <input className="auth_input error" type="password" placeholder="password" name="user-password-repeat"
                       id="user-password-repeat"/>
                  <a href="#" className="password-control"
                     onClick={() =>
                     {
                       let inp = document.getElementById('user-password-repeat');
                       inp.type = inp.type === "text" ? "password" : "text"
                     }}></a>
              </div>
              <label className="sign-in__label visually-hidden" htmlFor="user-password-repeat">Повторите пароль</label>
              <p className="input-error__text visually-hidden" id="password-repeat-error">Пароли не совпадают</p>
            </div>
            {/*<div className="auth__field password">*/}
            {/*  <p className="error">Пароль</p>*/}
            {/*  <input className="auth_input error" type="password" placeholder="password" name="user-password" id="user-password" />*/}
            {/*  <label className="sign-in__label visually-hidden" htmlFor="user-password">Пароль</label>*/}
            {/*  <p className="input-error__text visually-hidden" id="password-error">Длина пароля должна быть от 6 символов</p>*/}
            {/*</div>*/}

            {/*<div className="auth__field password-repeat">*/}
            {/*  <p className="error">Повторите пароль</p>*/}
            {/*  <input className="auth_input error" type="password" placeholder="password" name="user-password-repeat" id="user-password-repeat" />*/}
            {/*  <label className="sign-in__label visually-hidden" htmlFor="user-password-repeat">Повторите пароль</label>*/}
            {/*  <p className="input-error__text visually-hidden" id="password-repeat-error">Пароли не совпадают</p>*/}
            {/*</div>*/}


          </div>

          <button className="auth__btn" type="button" onClick={() => {sendData("customer")}}>Зарегистрироваться</button>
        </div>
      </div>
    </form>
    )
}

export default CustomerForm;
