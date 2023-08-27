import Content from "./content";
import Form from "./form";

const SignIn = () => {
    return (
        <div className="auth-wrapper">
            <div className="sign-in">
                <Content />
                <Form />
            </div>
        </div>
    );
}

export default SignIn;