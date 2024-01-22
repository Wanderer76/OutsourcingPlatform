import React from "react";
import {formatTagValue} from "../../tasks/format-tag";

const CompetenceBlock = (props) => {
    const competencies = props.competencies.length === 0 ? null : props.competencies.map((comp) =>
        <div key={"comp-"+comp.id} className="field-tag">{formatTagValue(comp.name)}</div>
    )

    const skills = props.skills.length === 0 ? null : props.skills.map((skill) =>
        <div key={"skill-"+skill.id} className="field-tag">{formatTagValue(skill.name)}</div>
    )

    return (
        <>
        {skills !== null || competencies !== null ?
            <section className="user-competencies">
                <h2>Мои роли:</h2>
                <div className="competences">
                    {competencies !== null ?
                        <div className="user-fields">
                            <h3>Сферы деятельности:</h3>
                            <div className="fields-tags">
                                {competencies}
                            </div>
                        </div> :
                            <></>
                    }
                    {skills !== null ?
                        <div className="user-skills">
                            <h3>Компетенции:</h3>
                            <div className="fields-tags">
                                {skills}
                            </div>
                        </div> : <></>
                    }
                </div>
            </section> : <></>
        }
        </>
    );
}

export default CompetenceBlock;