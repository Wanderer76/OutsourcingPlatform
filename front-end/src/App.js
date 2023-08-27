import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./css/css.css"
import SignUp from "./components/sign-up/sign-up";
import SignIn from "./components/sign-in/sign-in";
import MainRoutes from "./components/main/MainRoutes";
function App() {

  return(
        <BrowserRouter>
          <Routes>
              <Route path="/signin" element={<SignIn />}/>
              <Route path="/signup" element={<SignUp />}/>
              <Route path="/*" element={<MainRoutes/>}/>
          </Routes>
        </BrowserRouter>
  );
}

export default App;
