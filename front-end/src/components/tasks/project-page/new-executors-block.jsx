import NewExecutorCard from "./new-executor-card";
import {useEffect, useState} from "react";
import ExecutorCard from "./executor-card";

const NewExecutorsBlock = (props) => {

    const [visible, setVisible] = useState(true);
    const [offset, setOffset] = useState(0);
    const [executorsCards, setExecutorCards] = useState([])
    const limit = 5;

    useEffect(getMore, []);

    function getMore() {
        let executors = props.executors.map((executor, index) => {return <NewExecutorCard info={executor} key={executor.executorId}/>})
        setOffset(offset + 1)
        setExecutorCards([...executorsCards, ...executors]);
    }

    return(
        <section className="project-new-contractors">
            <div className="project-new-contractors subtitle-row">
                <input className="project-new-contractors show-hide-list" type="button" id="new-contractors-btn" onClick={(e) => {
                    setVisible(!visible);
                    e.target.classList.toggle("hide");
                }}/>
                    <h2 className="page-subtitle">Новые предложения:</h2>
            </div>
            <div className="project-new-contractors contractor-cards">
                {visible ?
                    <>{executorsCards}
                        {props.executors.length > offset * limit ?
                            <input className="more-cards-btn" type="button" value="Показать больше" onClick={getMore}/> : <></> }
                    </> :
                    <></>
                }

            </div>
        </section>
    )
}

export default NewExecutorsBlock;