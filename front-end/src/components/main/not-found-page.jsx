import Header from "./header";
import Footer from "./footer";
import NotFoundImage from "../../images/not-found.svg"

const NotFoundPage = () => {
    return(
            <section className="not-found-section">
                <img className="not-found-image" src={NotFoundImage} width="380px" height="190px"
                     alt="not-found"/>
                    <a className="project-card-btn" href="/">На главную страницу</a>
            </section>
    )
}

export default NotFoundPage;