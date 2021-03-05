using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace O2ArchiveReader.ViewModels
{
    public abstract class ObservableObject : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected virtual bool SetProperty<T>(
			ref T backingStore, T value,
			[CallerMemberName] string propertyName = "",
			Action onChanged = null,
			Func<T, T, bool> validateValue = null)
		{
			//if value didn't change
			if (EqualityComparer<T>.Default.Equals(backingStore, value))
				return false;

			//if value changed but didn't validate
			if (validateValue != null && !validateValue(backingStore, value))
				return false;

			backingStore = value;
			RaisePropertyChanged(propertyName);
			onChanged?.Invoke();
			return true;
		}
	}
}
