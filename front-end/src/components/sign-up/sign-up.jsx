import { useState } from "react";
import ConstructorForm from "./constructor-form";
import SingUpContent from "./content"
import CustomerForm from "./customer-form";
import { customerRole, constructorRole } from "./constants";

const SignUp = () => {
        const [role, setRole] = useState(constructorRole);
        return(
            <div className="auth-wrapper">
                <div className="sign-up">
                    <SingUpContent onChangeFunc = {changeRole} role={role}/>
                    {role === customerRole ?
                    <CustomerForm/> :
                    <ConstructorForm />
                    }
                </div>
            </div>
        );

        function changeRole() {
            if (role === customerRole)
                setRole(constructorRole);
            else
                setRole(customerRole);
        }
}

export default SignUp;

