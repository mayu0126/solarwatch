import PropTypes from 'prop-types';

const SolarWatchFeature = ({ onSave, disabled, errorMessage, isErrorMessageVisible }) => {

    const onSubmit = (e) => {
        e.preventDefault();
        const formData = new FormData(e.target);
        const entries = [...formData.entries()];
    
        const solarDataRequest = entries.reduce((acc, entry) => {
          const [k, v] = entry;
          acc[k] = v;
          return acc;
        }, {});
    
        return onSave(solarDataRequest);
    };

    return (
    <>
    <form className="SolarWatchForm" onSubmit={onSubmit}>

        {<div className="control">
        <label htmlFor="cityName">CITY:</label>
        <input
            className="input-field"
            name="cityName"
            id="cityName"
        />
        </div>}

        <div className="control">
        <label htmlFor="date">DATE:</label>
        <input
            className="input-field"
            pattern="\d{4}-\d{2}-\d{2}" maxLength="10"
            placeholder="YYYY-MM-DD"
            name="date"
            id="date"
        />
        </div>

        <button className="submit-button" type="submit" disabled={disabled}>
            Get sunrise and sunset
        </button>
    </form>
    {isErrorMessageVisible && <p className="error-message">{errorMessage}</p>}
    </>
    );
};

SolarWatchFeature.propTypes = {
    onSave: PropTypes.func.isRequired,
    disabled: PropTypes.bool.isRequired,
    errorMessage: PropTypes.string
};

export default SolarWatchFeature;