import ReactDOM from 'react-dom';
import React from 'react';
import serialize from 'form-serialize';

export default ({ app, appLayout }) => (req, res, next) => {
  const Link = (props) => {
    const onClick = (e) => {
      e.preventDefault();
      app.navigate(e.target.href);
    };
    const mergedProps = { onClick, ...props };
    return React.createElement('a', mergedProps);
  };

  req.Link = Link;

  const Form = (props) => {
    const onSubmit = (e) => {
      e.preventDefault();
      const body = serialize(e.target, { hash: true });
      app.submit(e.target.action, e.target.method, body);
    };
    const mergedProps = { onSubmit, ...props };
    const { children } = mergedProps;
    delete mergedProps.children;
    const formElements = [].concat(children);
    formElements.push(
      React.createElement('input', { type: 'hidden', name: '_csrf', value: req.csrf })
    );
    return React.createElement('form', mergedProps, formElements);
  };

  req.Form = Form;

  res.renderComponent = (content, options = {}) => {
    const { title, description } = options;
    const statusCode = options.statusCode || 200;
    const layout = options.layout || appLayout;
    const { appContainer } = req.renderDocument({ title, description });
    ReactDOM.hydrate(React.createElement(layout, { content, req }), appContainer, () => {
      res.status(statusCode);
      res.send();
    });
  };

  next();
};
