import Header from "./header";
import Footer from "./footer"
import heroImage from "../../images/hero-image.jpg"
import emoji1 from "../../images/emoji-1.png"
import emoji2 from "../../images/emoji-2.png"
import emoji3 from "../../images/emoji-3.png"
import emoji4 from "../../images/emoji-4.png"
import emoji5 from "../../images/emoji-5.png"
import emoji6 from "../../images/emoji-6.png"
import emoji7 from "../../images/emoji-7.png"
import emoji8 from "../../images/emoji-8.png"
import emoji9 from "../../images/emoji-9.png"
import emoji10 from "../../images/emoji-10.png"


const MainPage = () => {
    return(
    <div className="main">
        <section className="hero-section">
            <div className="hero-block">
                <div className="hero-photo">
                    <img src={heroImage} alt="Фотография девушки"/>
                </div>
                <div className="hero-content">
                    <h1><b>Первый старт</b> — в вашу карьеру</h1>
                    <div className="hero-text">
                        <p>Добро пожаловать в <b>Первый старт</b>!</p>
                        <p>Мы — платформа для взаимодействия крутых людей.</p>
                        <p>Кто-то ищет возможность получить первый опыт в реальных проектах в начале профессиональной деятельности.</p>
                        <p>Кому-то нужна помощь в решении важных задач в бизнесе при ограниченном количестве ресурсов.</p>
                        <p>Мы помогаем потенциальным талантливым исполнителям и заказчикам найти друг друга — <b>запустить</b> карьеру!</p>
                    </div>
                    <input className="to-sign-up-btn" type="button" value="Зарегистрироваться" onClick={() => {window.location.href = "/signup"}}/>
                </div>
            </div>
        </section>
        <section className="hero-cards-section">
            <h2>Подробнее о первом старте</h2>
            <div className="hero-cards">
                <div className="hero-card">
                    <h3>Правила взаимодействия в сообществе</h3>
                    <div className="card-desc">
                        <p>Наша миссия — помогать людям без опыта начинать карьеру.</p>
                        <p>Мы выделяем две категории пользователей: исполнители и заказчики.</p>
                        <p>Почитайте о том, в чем заключается роль каждого из них...</p>
                    </div>
                    <a className="card-button" href="/404">О наших правилах</a>
                </div>
                <div className="hero-card">
                    <h3>Почему вы должны стать исполнителем?</h3>
                    <div className="card-desc">
                        <p>Если вы ищите первую работу, но у вас нет практического опыта — станьте членом нашего сообщества!</p>
                        <p>Принимайте участие в проектах наших заказчиков — приумножайте свой опыт и собирайте портфолио для будуших собеседований!</p>
                    </div>
                    <a className="card-button" onClick={() => {
                        document.getElementById("for-contractors").scrollIntoView({block: "center", behavior: "smooth"})
                    }}>Об исполнителях</a>
                </div>
                <div className="hero-card">
                    <h3>Почему вы должны стать заказчиком?</h3>
                    <div className="card-desc">
                        <p>Если у вас есть свое небольшое дело, и бюджет не позволяет обратиться к помощи профессиональных агенств — присоединяйтесь к нам!</p>
                        <p>Наши начинающие исполнители помогут решить многие проблемы. И это бесплатно!</p>
                    </div>
                    <a className="card-button" onClick={() => {
                        document.getElementById("for-customers").scrollIntoView({block: "center", behavior: "smooth"})
                    }}>О заказчиках</a>
                </div>
            </div>
        </section>
        <section className="for-contractors" >
            <h2 id="for-contractors">Почему вы должны стать <br/>нашим исполнителем?</h2>
            <div className="for-contractors-cards">
                <div className="for-contractors-card">
                    <div className="card-img">
                        <img className="icon" src={emoji1} alt="emoji"/>
                    </div>
                    <div className="card-content">
                        <h3>Не бойтесь делать первые шаги в профессию</h3>
                        <div className="card-desc">
                            <p>Вы — начинающий специалист, и заказчики знают это, ведь в этом философия нашей платформы.</p>
                            <p>Не бойтесь совершать ошибки, практикуйтесь и становитесь лучше!</p>
                        </div>
                    </div>
                </div>
                <div className="for-contractors-card">
                    <div className="card-img">
                        <img className="icon" src={emoji2} alt="emoji"/>
                    </div>
                    <div className="card-content">
                        <h3>Собирайте портфолио</h3>
                        <div className="card-desc">
                            <p>Выполняйте проекты на платформе и приумножайте свою копилку работ, чтобы потом показывать их на собеседованиях.</p>
                        </div>
                    </div>
                </div>
                <div className="for-contractors-card">
                    <div className="card-img">
                        <img className="icon" src={emoji3} alt="emoji"/>
                    </div>
                    <div className="card-content">
                        <h3>Прокачивайте свои hard-skills</h3>
                        <div className="card-desc">
                            <p>Вы Освоили знание в новом деле? — Пришло время продемонстрировать его на реальных проектах.</p>
                            <p>Прокачивайте свои навыки на кейсах от наших заказчиков!</p>
                        </div>
                    </div>
                </div>
                <div className="for-contractors-card">
                    <div className="card-img">
                        <img className="icon" src={emoji4} alt="emoji"/>
                    </div>
                    <div className="card-content">
                        <h3>Развивайте soft-skills</h3>
                        <div className="card-desc">
                            <p>Прокачивайте навык коммуникации с заказчиком и деловой этикет.</p>
                            <p>Учитесь выявлять потребности клиента и не забывайте про тайм-менеджмент — ведь к каждому проекту есть свой дедлайн</p>
                        </div>
                    </div>
                </div>
                <div className="for-contractors-card">
                    <div className="card-img">
                        <img className="icon" src={emoji5} alt="emoji" />
                    </div>
                    <div className="card-content">
                        <h3>Развивайте soft-skills</h3>
                        <div className="card-desc">
                            <p>По завершении каждого проекта вы получаете полноценный отзыв на вашу работу.</p>
                            <p>Также по инициативе заказчика вы можете получить благодарственные письма от компаний, символическое вознаграждение, персональные скидки и другие приятные бонусы к проделанной вами работе.</p>
                        </div>
                    </div>
                </div>
            </div>
        </section>
        <section className="for-customers" >
            <h2 id="for-customers">Почему вы должны стать <br/>нашим заказчиком?</h2>
            <div className="for-customers-cards">
                <div className="for-customers-card">
                    <div className="card-img">
                        <img className="icon" src={emoji9} alt="emoji"/>
                    </div>
                    <div className="card-content">
                        <h3>Для вас — бесплатно</h3>
                        <p>Для исполнителя ваша задача — это бесценный опыт, для будущей профессии.</p>
                        <p>Для вас результат — старания начинающего профессионала. Да, возможно это не самый идеальный результат — зато бесплатно!</p>
                    </div>
                </div>
                <div className="for-customers-card">
                    <div className="card-img">
                        <img className="icon" src={emoji8} alt="emoji"/>
                    </div>
                    <div className="card-content">
                        <h3>У вас небольшое дело...</h3>
                        <p>...но ресурсов на хорошего дизайнера или программиста не хватает.</p>
                        <p>Тогда создавайте кейсы на нашей платформе и принимайте заявки от начинающих профессионалов — они помогут вам сверстать небольшой сайт-лэндинг или, например, нарисовать логотип вашей компании и многое другое...</p>
                    </div>
                </div>
                <div className="for-customers-card">
                    <div className="card-img">
                        <img className="icon" src={emoji10} alt="emoji"/>
                    </div>
                    <div className="card-content">
                        <h3>Одна задача — несколько результатов</h3>
                        <p>Вы можете принимать несколько исполнителей к своему заказу и выбрать для себя работу, которая вам больше всего понравилась.</p>
                        <p>Но вы должны обязательно дать отзыв каждому из исполнителей после окончания проекта.</p>
                    </div>
                </div>
            </div>
        </section>
    </div>
    )
}

export default MainPage;
