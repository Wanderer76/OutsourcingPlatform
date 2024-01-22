import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./css/css.css"
import SignUp from "./components/sign-up/sign-up";
import SignIn from "./components/sign-in/sign-in";
import MainRoutes from "./components/main/MainRoutes";
import {VerifyPage} from "./components/sign-up/verify-page";
import {Verification} from "./components/sign-up/verification";
function App() {

  return(
        <BrowserRouter>
          <Routes>
              <Route path="/signin" element={<SignIn />}/>
              <Route path="/signup" element={<SignUp />}/>
              <Route path="/verify-page" element={<VerifyPage />}/>
              <Route path="/verification" element={<Verification />}/>
              <Route path="/*" element={<MainRoutes/>}/>
          </Routes>
        </BrowserRouter>
  );
}

export default App;
