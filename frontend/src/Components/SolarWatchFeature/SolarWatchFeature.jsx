import PropTypes from 'prop-types';

const SolarWatchFeature = ({ onSave, disabled, errorMessage }) => {

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
        <label htmlFor="cityName">City:</label>
        <input
            name="cityName"
            id="cityName"
        />
        </div>}

        <div className="control">
        <label htmlFor="date">Date:</label>
        <input
            name="date"
            id="date"
        />
        </div>

        <div className="buttons">
        <button type="submit" disabled={disabled}>
            Get sunrise and sunset
        </button>
        </div>
    </form>
    {errorMessage && <p className="error-message">{errorMessage}</p>}
    </>
    );
};

SolarWatchFeature.propTypes = {
    onSave: PropTypes.func.isRequired,
    disabled: PropTypes.bool.isRequired,
    errorMessage: PropTypes.string
};

export default SolarWatchFeature;