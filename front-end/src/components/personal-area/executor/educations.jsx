const Educations = (props) => {
    console.log(props)
    let edus = props.educations.map((edu) =>
        <div key={'edu-'+edu.id} className="education-tab-row">
            <div className="place">{edu.place}</div>
            <div className="specialty">{edu.speciality}</div>
            <div className="graduation-year">{edu.graduationYear}</div>
        </div>
    );

      return (
          <>
              {edus.length !== 0 ?
                  <section className="user-educations">
                      <h2>Образование:</h2>
                      <div className="education-tab">
                          <div className="education-tab-row head">
                              <div className="place">Место</div>
                              <div className="specialty">Специальность</div>
                              <div className="graduation-year">Год окончания</div>
                          </div>
                          {edus}
                      </div>
                  </section> :
                  <></>}

          </>
      );
    }

export default Educations;