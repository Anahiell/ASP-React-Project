import React, { useState, useEffect } from 'react';
import { Navbar, Nav, Container, Dropdown } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import '../css/Layout.css'; 
import logo from '../img/logo1.png';


function Layout({ children }) {
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    // Функция эмулирует вход пользователя
    const handleLogin = () => {
        setIsAuthenticated(true);
        // В реальном приложении здесь должна быть логика для входа пользователя
    };

    // Функция эмулирует выход пользователя
    const handleLogout = () => {
        setIsAuthenticated(false);
        //  здесь должна быть логика для выхода пользователя
    };

    useEffect(() => {
        //  логика для проверки аутентификации при загрузке компонента
        //  можно добавить проверку с использованием токенов или сессий
        // Например localStorage или sessionStorage
        const userLoggedIn = localStorage.getItem('loggedIn');
        if (userLoggedIn) {
            setIsAuthenticated(true);
        }
    }, []);
        const [isOpen, setIsOpen] = useState(false);
    
        const handleToggle = () => {
            setIsOpen(!isOpen);
        };
    
        const handleItemClick = (eventKey) => {
            console.log(`Clicked on ${eventKey}`);
        };
    return (
        <div>
            <header>
            <Navbar className="custom-navbar" expand="lg">
                <Container>
                    <Navbar.Brand as={Link} to="/" className="custom-navbar-brand">
                    <img
                                src={logo}
                                width="50"
                                height="50"
                                className="custom-logo d-inline-block align-top"
                                alt="Logo"
                            />
                    </Navbar.Brand>
                    <Navbar.Toggle aria-controls="basic-navbar-nav" className="custom-navbar-toggle" />
                    <Navbar.Collapse id="basic-navbar-nav" className="custom-navbar-collapse">
                        <Nav className="me-auto custom-nav">
                        <Dropdown 
            show={isOpen}
            onMouseEnter={() => setIsOpen(true)}
            onMouseLeave={() => setIsOpen(false)}
            onClick={() => setIsOpen(!isOpen)}
            className="custom-dropdown"
        >
            <Dropdown.Toggle
                as={Link}
                to="/products"
                variant="light"
                id="dropdown-basic"
                className="custom-dropdown-toggle"
            >
                Магазин
            </Dropdown.Toggle>
            <Dropdown.Menu className="custom-dropdown-menu">
                <Dropdown.Item href="/products/t-shirts" onClick={() => handleItemClick('Футболки')}>
                    Футболки
                </Dropdown.Item>
                <Dropdown.Item href="/products/accessories" onClick={() => handleItemClick('Аксессуары')}>
                    Аксессуары
                </Dropdown.Item>
                <Dropdown.Item href="/products/underwear" onClick={() => handleItemClick('Нижнее белье')}>
                    Нижнее белье
                </Dropdown.Item>
                <Dropdown.Item href="/products/pants" onClick={() => handleItemClick('Штаны')}>
                    Штаны
                </Dropdown.Item>
            </Dropdown.Menu>
        </Dropdown>
                            <Nav.Link as={Link} className="custom-nav-link">Акции</Nav.Link>
                            <Nav.Link as={Link} to="/about" className="custom-nav-link">О нас</Nav.Link>
                        </Nav>
                        <Nav className="custom-auth-nav">
                            {isAuthenticated ? (
                                <>
                                    <Nav.Link href="/favorites" className="custom-nav-link">
                                        <i className="bi bi-heart"></i> Любимые вещи
                                    </Nav.Link>
                                    <Nav.Link href="/cart" className="custom-nav-link">
                                        <i className="bi bi-cart"></i> Корзина (0)
                                    </Nav.Link>
                                    <Nav.Link href="/profile" className="custom-nav-link">
                                        <img
                                            src="/img/avatars/user-avatar.png"
                                            className="rounded-circle avatar"
                                            alt="User Avatar"
                                        />
                                        John Doe
                                    </Nav.Link>
                                    <Nav.Link onClick={handleLogout} className="custom-nav-link">Выйти</Nav.Link>
                                </>
                            ) : (
                                <Nav.Link href="#" data-bs-toggle="modal" data-bs-target="#authModal" className="custom-nav-link">
                                    <i className="bi bi-person"></i> Войти
                                </Nav.Link>
                            )}
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
        </header>

            <div className="container">
                <main role="main" className="pb-3">
                    {children} {/* Здесь будет рендериться основное содержимое страницы */}
                </main>
            </div>

            <footer className="border-top footer text-muted mt-5">
                <div className="container">
                    &copy; 2024 - Azure221Spu -{' '}
                    <a href="/privacy">Политика конфиденциальности</a>
                </div>
            </footer>

            {/* Модальное окно для аутентификации */}
            <div className="modal fade" id="authModal" tabIndex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title" id="exampleModalLabel">Аутентификация (Войти в систему)</h5>
                            <button type="button" className="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                        </div>
                        <div className="modal-body">
                            <div className="col">
                                <div className="input-group mb-3">
                                    <span className="input-group-text" id="auth-email-addon">
                                        <i className="bi bi-envelope"></i>
                                    </span>
                                    <input
                                        type="text"
                                        className="form-control"
                                        placeholder="Введите email"
                                        aria-label="Username"
                                        id="auth-email"
                                        aria-describedby="auth-email-addon"
                                    />
                                </div>
                            </div>
                            <div className="col">
                                <div className="input-group mb-3">
                                    <span className="input-group-text" id="auth-password-addon">
                                        <i className="bi bi-unlock"></i>
                                    </span>
                                    <input
                                        type="password"
                                        className="form-control"
                                        placeholder="Введите пароль..."
                                        aria-label="Password"
                                        id="auth-password"
                                        aria-describedby="auth-password-addon"
                                    />
                                </div>
                            </div>
                        </div>
                        <div className="modal-footer">
                            <div id="auth-warning" className="alert alert-warning visually-hidden" role="alert"></div>
                            <a className="btn btn-secondary" href="/register">
                                Регистрация
                            </a>
                            <button type="button" className="btn btn-primary" onClick={handleLogin}>
                                Войти
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Layout;
