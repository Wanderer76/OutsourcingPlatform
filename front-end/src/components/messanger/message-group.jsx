import MessageItem from "./message-item";

const MessageGroup = ({date, messages, onMouseEnter, onClickHandler}) => {
    return (
        <>
            <div className="day-tag">{date}</div>
            {messages.map((message) => <MessageItem key={message.id} info={message} onMouseEnter={(e) => {
                if (!message.isViewed)
                    onMouseEnter(message.id)
                // .then(() => e.target.classList.remove('message-item-new'));
            }}/>)}
        </>
    )
}

export default MessageGroup;