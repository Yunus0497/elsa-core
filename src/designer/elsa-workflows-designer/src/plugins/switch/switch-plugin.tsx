import 'reflect-metadata';
import {h} from '@stencil/core';
import {Container, Service} from "typedi";
import {ActivityDriverRegistry, ActivityIconRegistry, InputControlRegistry} from "../../services";
import {Plugin} from "../../models";
import {SwitchActivityDriver} from "./switch-activity-driver";
import {PortProviderRegistry} from "../../components/activities/flowchart/port-provider-registry";
import {SwitchPortProvider} from "./switch-port-provider";
import {SwitchPortUpdater} from "./switch-port-updater";
import {TransposeHandlerRegistry} from "../../components/activities/flowchart/transpose-handler-registry";
import {SwitchTransposeHandler} from "./switch-transpose-handler";


@Service()
export class SwitchPlugin implements Plugin {
  public static readonly ActivityTypeName: string = 'Elsa.Switch';

  constructor() {
    const activityTypeName = SwitchPlugin.ActivityTypeName;
    const driverRegistry = Container.get(ActivityDriverRegistry);
    const inputControlRegistry = Container.get(InputControlRegistry);
    const portProviderRegistry = Container.get(PortProviderRegistry);
    const switchPortUpdater = Container.get(SwitchPortUpdater);
    const transposeHandlerRegistry = Container.get(TransposeHandlerRegistry);
    const iconRegistry = Container.get(ActivityIconRegistry);

    //driverRegistry.add(activityTypeName, () => Container.get(SwitchActivityDriver));
    inputControlRegistry.add('switch-editor', c => <elsa-switch-editor inputContext={c}/>);
    portProviderRegistry.add(activityTypeName, () => Container.get(SwitchPortProvider));
    transposeHandlerRegistry.add(activityTypeName, () => Container.get(SwitchTransposeHandler));
    iconRegistry.add(SwitchPlugin.ActivityTypeName, Icon);
  }
}

const Icon: () => string = () =>
  `<svg class="h-6 w-6 text-white" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
      <path stroke="none" d="M0 0h24v24H0z"/>
      <polyline points="15 4 19 4 19 8"/>
      <line x1="14.75" y1="9.25" x2="19" y2="4"/>
      <line x1="5" y1="19" x2="9" y2="15"/>
      <polyline points="15 19 19 19 19 15"/>
      <line x1="5" y1="5" x2="19" y2="19"/>
    </svg>`;