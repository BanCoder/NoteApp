import {FC, ReactElement} from 'react'; 
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import './App.css';
import userManager, {
  loadUser, 
  signinRedirect, 
  signoutRedirect
} from './auth/user-service'
import AuthProvider from './auth/auth-provider';
import SigninOidc from './auth/SigninOidc';
import SignoutOidc from './auth/SignoutOidc';
import NoteList from './notes/NoteList';

const App: FC<{}> =(): ReactElement => {
  loadUser(); 
  return (
    <div className="App">
      <header className="App-header">
      <button onClick={() => signinRedirect()}>Login</button>
      <AuthProvider userManager={userManager}>
        <Router>
          <Routes>
            <Route path='/' element={<NoteList />} />
            <Route 
                path='/signout-oidc'
                element={<SignoutOidc />}
                />
            <Route path='/signin-oidc' element={<SigninOidc />} />
          </Routes>
        </Router>
      </AuthProvider>
      </header>
    </div>
  );
}

export default App;
