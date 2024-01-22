const CompanyInfo = (props) => {
  return (
      <section className="company-data">
          <h2>Сведения об организации:</h2>
          <div className="company-card">
              <p className="company-name">{props.info['companyName']}</p>
              <p className="company-address">{"Адрес: " + props.info.address}</p>
              <p className="company-inn">{"ИНН: " + props.info['inn']}</p>
          </div>

      </section>
  )
}

export default CompanyInfo;