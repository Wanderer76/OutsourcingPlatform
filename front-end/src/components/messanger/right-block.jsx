import avatar from "../../images/notification-avatar.svg"
import MessageItem from "./message-item";
import MessageGroup from "./message-group";
import {sortMessages} from "./sort-messages";
import {useEffect, useRef, useState} from "react";

const RightBlock = ({messages, onMouseEnter, onSubmit, getMoreMessage}) => {
    const [scrollOffset, setScrollOffset] = useState(0);
    //let sortedMessages = sortMessages(messages)
    const myref = useRef();
    useEffect(() => {
        // console.log(myref.current)
        // if (myref && !myref.current.onScroll)
            // myref.current.addEventListener("scroll", (e) => {scrolling(e)})
        if (myref && myref.current.scrollTop > myref.current.scrollHeight - myref.current.clientHeight - 200)
            myref.current.scrollTop = myref.current.scrollHeight;
        // console.log("clientHeight" + myref.current.clientHeight)
        // console.log("scrollHeight" + myref.current.scrollHeight)
        // console.log("scrollTop" + myref.current.scrollTop)
    }, [messages])

    // useEffect(() => {
    //     if (myref && myref.current) {
    //         myref.addEventListener("scroll", scrolling)
    //         myref.current.scrollTop = myref.current.scrollHeight;
    //     }
    //
    // }, [myref])

    useEffect(() => {
        if (myref && myref.current) {
            myref.current.addEventListener("scroll", (e) => {scrolling(e)})
        }
    }, [])

    const scrolling = (e) => {
        //it's work, here should be call message pagination function)
        if (myref.current.scrollTop === 0) {
            // console.log("end block \n =======================")
            getMoreMessage(10);
        }
    }

    // useEffect(() => {
    //     console.log("Scrolling")
    // }, [myref])

    return(
        <div className="messenger-right-block">
            <div className="opened-chat" ref={myref}>
                {sortMessages(messages).map((pair) => {
                    return <MessageGroup key={pair[0]} date={pair[0]} messages={pair[1]} onMouseEnter={onMouseEnter}/>

                })}


            </div>
            {window.sessionStorage.role !== "admin_role" &&
                <div className="message-input-block">
                    <form className="message-input" id="message-input">
                        <textarea id="message" rows="1" placeholder="Написать сообщение..." onKeyUp={e => {
                            if(e.key === 'Enter') {
                                onSubmit(e.target.value)
                                e.target.value = "";
                            }
                        }}></textarea>
                        <button type="button" className="message-send-btn" id="message-send-btn"
                            onClick={e => {
                                onSubmit(document.getElementById('message').value)
                                document.getElementById("message").value = "";
                        }}>
                        </button>
                    </form>
                </div>}
        </div>
    )
}

export default RightBlock;