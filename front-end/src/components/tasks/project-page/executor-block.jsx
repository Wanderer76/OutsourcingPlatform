import ExecutorCard from "./executor-card";
import {useEffect, useState} from "react";

const ExecutorBlock = (props) => {
    const [visible, setVisible] = useState(true);
    const [offset, setOffset] = useState(0);
    const [executorsCards, setExecutorCards] = useState([])
    const limit = 5;

    useEffect(getMore, []);

    function getMore() {
        console.log(props)
        let executors = props.executors.slice(offset * limit, (offset + 1) * limit)
            .map((executor) => {
                return <ExecutorCard info={executor} orderId={props.orderId} isCompleted={props.isCompleted}
                                     key={executor.executorId}/>
            })
        setOffset(offset + 1)
        setExecutorCards([...executorsCards, ...executors]);
    }

    return (
        <section className="project-contractors">
            <div className="project-contractors subtitle-row">
                <input className="project-contractors show-hide-list" type="button" id="contractors-btn"
                       onClick={(e) => {
                           setVisible(!visible);
                           e.target.classList.toggle("hide");
                       }}/>
                <h2 className="page-subtitle">Исполнители заказа:</h2>
            </div>
            <div className="project-contractors contractor-cards">
                {
                    visible ? <>
                            {executorsCards}
                            {props.executors.length > offset * limit ?
                                <input className="more-cards-btn" type="button" value="Показать больше"
                                       onClick={getMore}/> : <></>} </> :
                        <></>
                }


            </div>
        </section>

    )
}

export default ExecutorBlock;