%create cm via the MATLAB confusionmat function
class_count = 90;
l = sum(cm,'all');
tp = diag(cm);
fp = sum(cm,2)-tp;
fn = sum(cm,1)'-tp;
tn = l-tp-fp-fn;
error_rate_micro_macro = sum((fp+fn)./(tp+fn+fp+tn))./class_count;
accuracy_micro_macro = sum(tp+tn)./sum(tp+tn+fp+fn);
true_negative_rate_micro = sum(tn)./sum(tn+fp);
true_positive_rate_micro = sum(tp)./sum(tp+fn);
balanced_accuracy_micro = (true_negative_rate_micro+true_positive_rate_micro)./2;
true_negative_rate_macro = tn./(tn+fp);
true_positive_rate_macro = tp./(tp+fn);
balanced_accuracy_macro = sum(true_negative_rate_macro+true_positive_rate_macro)./2./class_count;
precision_micro = sum(tp)./sum(tp+fp);
precision_macro = sum(tp./(tp + fp))./class_count;
recall_micro=sum(tp)./sum(tp+fn);
recall_macro=sum(tp./(tp + fn))./class_count;
f1_score_micro = 2 * recall_micro * precision_micro/(recall_micro + precision_micro);
f1_score_macro = 2 * recall_macro * precision_macro/(recall_macro + precision_macro);